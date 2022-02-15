import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { LoginForm } from 'src/app/models/forms';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.scss'],
})
export class LoginModalComponent implements OnInit {
  model: LoginForm = {
    username: '',
    password: '',
  };

  constructor(
    public accountService: AccountService,
    private router: Router,
    private bsModalRef: BsModalRef
  ) {}

  login(): void {
    this.accountService
      .login(this.model)
      .subscribe(() =>
        this.router.navigateByUrl('/members')
      );
    this.closeDialog();
  }

  closeDialog() {
    this.bsModalRef.hide();
  }

  switchToRegister() {
    this.accountService.switchModalDialog(
      this.accountService.loginModalId
    );
  }

  ngOnInit(): void {}
}
