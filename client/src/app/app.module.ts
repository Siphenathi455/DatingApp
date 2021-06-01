import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from '../app.component';
import { NavComponent } from '../nav/nav.component';
import { HomeComponent } from '../home/home.component';
import { RegisterComponent } from '../register/register.component';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { MessagesComponent } from '../messages/messages.component';
import { MemberListComponent } from '../members/member-list/member-list.component';
import { MemberDetailComponent } from '../members/member-detail/member-detail.component';
import { ListsComponent } from '../lists/lists.component';
import { TestErrorsComponent } from '../Errors/test-errors/test-errors.component';
import { NotFoundComponent } from '../Errors/not-found/not-found.component';
import { ServerErrorComponent } from '../Errors/server-error/server-error.component';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PhotoEditorComponent } from '../members/photo-editor/photo-editor.component';
import { TextInputComponent } from '../_forms/text-input/text-input.component';
import { DateInputComponent } from '../_forms/date-input/date-input.component';
import { MemberMessagesComponent } from '../members/member-messages/member-messages.component';
import { AdminPanelComponent } from '../admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from '../_directives/has-roles.directive';
import { HttpClientModule } from '@angular/common/http';



import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    MemberListComponent,
    RegisterComponent,
    MemberDetailComponent,
    ListsComponent,
    MemberEditComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputComponent,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
