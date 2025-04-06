import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { FailureDialogComponent } from '../modals/failure-dialog/failure-dialog.component';

import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginError: string = '';
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private snackBar: MatSnackBar,
              private router: Router,
              private authService: AuthService,
              private dialog: MatDialog) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login(username, password).subscribe({
        next: (response) => {
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.log(error);

          if (error.status === 0) {
            this.loginError = 'Connection to the web API failed. Please check your network connection.';
            this.dialog.open(FailureDialogComponent, {
              data: { message: this.loginError }
            });
          } else {
            this.loginError = 'Invalid username or password';
            this.dialog.open(FailureDialogComponent, {
              data: { message: this.loginError }
            });
          }
        }
      });

      console.log('Login successful:', username, password);
      this.snackBar.open('Login successful!', 'Close', {
        duration: 2000,
        panelClass: ['success-snackbar']
      });
    } else {
      this.snackBar.open('Please fill out all fields correctly.', 'Close', {
        duration: 2000,
        panelClass: ['error-snackbar']
      });
    }
  }
}
