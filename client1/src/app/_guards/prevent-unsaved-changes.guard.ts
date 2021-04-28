import { Injectable } from '@angular/core';
import { CanActivateChild, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanActivateChild, CanDeactivate<unknown> {
  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  canDeactivate(
    component: MemberEditComponent): boolean  {
      if(component.editForm.dirty)
      {
        return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
      }
      return true;
  }
  
}
