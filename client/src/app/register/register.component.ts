import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    console.log("Inside register component function register");
    console.log(this.model);
    this.accountService.register(this.model).subscribe( response => {
      console.log(response);
      this.cancel();
    }, error => {
      console.log("Error Registering with account service");
      console.log(error);
    })
  }

  cancel() {
    console.log("Inside register component function cancel");
    this.cancelRegister.emit(false)
  }

}
