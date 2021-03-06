import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUserSource =  new BehaviorSubject<string[]>([]);
  onlineUser$ = this.onlineUserSource.asObservable();

  constructor(private toastr: ToastrService, private router: Router) { }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
  createHubConnection(user: User)
  {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
        .start()
        .catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', username => {
       this.onlineUser$.pipe(take(1)).subscribe( usernames =>{
        this.onlineUserSource.next([...usernames, username])
       })
      });

    this.hubConnection.on('UserIsOffLine', username => {
      this.onlineUser$.pipe(take(1)).subscribe( usernames =>{
        this.onlineUserSource.next([...usernames.filter(x => x !== username)])
      });

      this.hubConnection.on('GetOnlineUsers', (usernames: string[]) =>{
        this.onlineUserSource.next(usernames);
      });

      this.hubConnection.on('NewMessageRecieved', ({username, knownAs}) =>{
        this.toastr.info(knownAs+ ' has sent you a message!')
        .onTap
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/'+ username + '?tab =3'));
      })
     })
  }
}
  
