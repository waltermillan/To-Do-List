import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './services/auth.service';
import { GLOBAL_CONFIG } from './config/config.global';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = GLOBAL_CONFIG.appFullName;
  footerDetails = GLOBAL_CONFIG.appLegalName.replace('__YEAR__', GLOBAL_CONFIG.currentYear.toString());
  userName: string = '';

  constructor(public authService: AuthService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.userName$.subscribe(userName => {
      this.userName = userName;
    });
  }

  logout() {
    this.authService.logout();
  }

  navigateToHome(){
    this.router.navigate(['/home']);
  }
}
