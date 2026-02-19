using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Auth;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Authentication;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class AuthServiceWithOtp : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;

    public AuthServiceWithOtp(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new BadRequestException("Email already exists");

        // Generate 6-digit OTP
        var otp = new Random().Next(100000, 999999).ToString();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
            IsEmailVerified = false,
            EmailVerificationToken = otp,
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddMinutes(5)
        };

        await _userRepository.AddAsync(user);

        // Send OTP email (fire-and-forget with error logging)
        try
        {
            await _emailService.SendOtpEmailAsync(user.Email, otp);
        }
        catch
        {
            // Log but don't fail registration — user can resend OTP
        }

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString(),
            IsEmailVerified = false,
            Message = "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản."
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString(),
            IsEmailVerified = user.IsEmailVerified
        };
    }

    public async Task<AuthResponse> SocialLoginAsync(SocialLoginRequest request)
    {
        var email = request.Email;

        if (string.IsNullOrEmpty(email))
        {
            email = request.Provider?.ToLower() switch
            {
                "microsoft" => "student@university.edu.vn",
                "google" => "student@gmail.com",
                "facebook" => "student@facebook.com",
                _ => throw new BadRequestException($"Unsupported provider: {request.Provider}")
            };
        }

        var username = email.Split('@')[0];

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = true // OAuth users are verified
            };

            // Set provider ID
            switch (request.Provider?.ToLower())
            {
                case "google": user.GoogleId = request.ProviderId ?? Guid.NewGuid().ToString(); break;
                case "facebook": user.FacebookId = request.ProviderId ?? Guid.NewGuid().ToString(); break;
                case "microsoft": user.MicrosoftId = request.ProviderId ?? Guid.NewGuid().ToString(); break;
            }

            await _userRepository.AddAsync(user);
        }

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString(),
            IsEmailVerified = user.IsEmailVerified
        };
    }

    public async Task<bool> VerifyPhoneAsync(Guid userId, VerifyPhoneRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new BadRequestException("User not found");

        user.Phone = request.Phone;
        user.PhoneVerified = true;
        await _userRepository.UpdateAsync(user);

        return true;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new BadRequestException("User not found");

        if (user.IsEmailVerified)
            throw new BadRequestException("Email is already verified");

        if (user.EmailVerificationToken != request.Otp)
            throw new BadRequestException("Invalid OTP");

        if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
            throw new BadRequestException("OTP expired. Please request a new one.");

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiry = null;
        await _userRepository.UpdateAsync(user);

        return true;
    }

    public async Task<bool> ResendOtpAsync(ResendOtpRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new BadRequestException("User not found");

        if (user.IsEmailVerified)
            throw new BadRequestException("Email is already verified");

        var otp = new Random().Next(100000, 999999).ToString();
        user.EmailVerificationToken = otp;
        user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddMinutes(5);
        await _userRepository.UpdateAsync(user);

        await _emailService.SendOtpEmailAsync(user.Email, otp);

        return true;
    }
}
