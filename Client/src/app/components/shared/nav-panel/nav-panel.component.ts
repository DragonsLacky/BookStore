import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  BsModalRef,
  BsModalService,
} from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-nav-panel',
  templateUrl: './nav-panel.component.html',
  styleUrls: ['./nav-panel.component.scss'],
})
export class NavPanelComponent implements OnInit {
  loginModalRef: BsModalRef | undefined;
  currentUser$: Observable<User | null>;

  constructor(
    public accountService: AccountService,
    private modalService: BsModalService,
    private router: Router
  ) {
    this.currentUser$ = this.accountService.currentUser$;
  }

  ngOnInit(): void {}

  showLogin() {
    this.loginModalRef =
      this.accountService.openLoginModal();
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
