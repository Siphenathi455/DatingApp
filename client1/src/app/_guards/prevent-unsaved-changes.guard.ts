import { Injectable } from '@angular/core';
import { CanActivateChild, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_Services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanActivateChild, CanDeactivate<unknown> {

constructor(private confirmService: ConfirmService){}

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  canDeactivate(
    component: MemberEditComponent): Observable<boolean>| boolean {
      if(component.editForm.dirty)
      {
        return this.confirmService.confirm();
      }
      return true;
  }
  
}
