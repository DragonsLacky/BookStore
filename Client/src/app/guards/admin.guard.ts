import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { RolesEnum } from '../models/roles';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(
        (user: User | null) =>
          user?.roles.includes(RolesEnum.Admin) ||
          user?.roles.includes(RolesEnum.Mod) ||
          (!!this.toastr.error(
            'You do not have the required permision'
          ) &&
            false)
      )
    );
  }
}
