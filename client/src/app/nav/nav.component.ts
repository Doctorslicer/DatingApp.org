import { Component, OnInit } from '@angular/core';
import { BrowserTestingModule } from '@angular/platform-browser/testing';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  //currentUser$: Observable<User>;
  //loggedIn: boolean = false;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$;
  }

  isLoggedIn() : Boolean {
    var bRet:Boolean = false;
    // if ((this.accountService.currentUser$ | async) !=== null)
    // {
    //   bRet = true;
    // }
    return bRet;
  }

  login() {
    console.log("login function begining");

    this.accountService.login(this.model).subscribe( response => {
      console.log("Logged into the account service");
      console.log(response);
      this.router.navigateByUrl('/members');
      //this.loggedIn = true;
    },error => {
      //this.loggedIn = false;
      console.log("Error login service");
      console.log(error);
      this.toastr.error(error.error);
    }
    );
  }

  logout() {
    //this.loggedIn = false;
    this.accountService.logout();
    console.log("Logged out");
    this.router.navigateByUrl('/');
  }

  // getCurrentUser()
  // {
  //   this.accountService.currentUser$.subscribe(user => {
  //     this.loggedIn = !!user;
  //   }), error => {
  //     console.log(error);
  //   }
    
  // }

}
