import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule, RouterOutlet } from '@angular/router';
import { constants } from '../../utils/constants/app.contants';
import { UserService } from '../../shared/services/user.service';
import { ToastrService } from '../../shared/services/toastr.service';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Users } from '../../utils/models/users.model';
import { ApiResponse } from '../../utils/models/api-response.model';

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent implements OnInit{
  isSignInMode: boolean = true;
  username: string = '';
  password: string = '';
  passwordFieldType: string = 'password';
  isPasswordVisible: boolean = false;
  passwordVisibilityIcon: string = 'hidden-icon-dark.svg';
  user: Users = new Users();

  constructor(private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private toastrService: ToastrService) { }

  ngOnInit(): void {
    console.log("init logged");
      this.route.data.subscribe({
      next: (data) => {
        this.isSignInMode = data['mode'] === 'sign-in';
      }
    });

    console.log("init ended()");
  }

  toggleMode(): void {
    this.isSignInMode = !this.isSignInMode;
    const newMode = this.isSignInMode ? 'sign-in' : 'sign-up';
    this.router.navigate([`/auth/${newMode}`]);
  }

  togglePasswordVisibility(): void {
      this.isPasswordVisible = !this.isPasswordVisible;
  }

  onSubmit(): void {
    if (this.isSignInMode) {
      this.signIn();
    } else {
      this.signUp();
    }
  }

  signIn(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response: ApiResponse<string>) => {
        if (response && response.data) {
          localStorage.setItem(constants.AUTHTOKEN_KEY, response.data);
          this.userService.getUserByName(this.username).subscribe({
            next: (userResponse: ApiResponse<Users>) => {
              if (userResponse && userResponse.data) {
                localStorage.setItem(constants.USER_ID, userResponse.data.id.toString());
                localStorage.setItem(constants.USERNAME, userResponse.data.username);
              }
              else {
                this.toastrService.showError("User details not received.");
              }
              this.toastrService.showSuccess("Login Successful");
              this.router.navigate(['/main/dashboard']);
            },
            error: () => {
              this.toastrService.showError("Error fetching user details, please try again");
            }
          });
        } else {
          this.toastrService.showError("Login failed. Please fill correct details");
        }
      },
      error: () => {
        this.toastrService.showError("Login failed. Please fill correct details");
      }
    });
  }

  signUp(): void {
    this.user.username = this.username;
    this.user.passwordHash = this.password;
    this.authService.register(this.user).subscribe({
      next: () => {
        this.toastrService.showSuccess('User created successfully');
        this.router.navigate(['/auth/sign-in']);
      },
      error: () => {
        this.toastrService.showError("Error occurred during SignUp");
      }
    });
  }
}
