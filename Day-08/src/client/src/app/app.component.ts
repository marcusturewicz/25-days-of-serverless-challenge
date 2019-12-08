import { HttpClient } from '@angular/common/http';
import { Status } from './status'
import { environment } from 'src/environments/environment';
import { HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';
  public status: Status;


  constructor(private http: HttpClient) {

  }

  ngOnInit() {
    this.http.get<Status>(environment.listIncidentsUrl).subscribe((data: Status) => {
      // Initially get the status
      this.status = data;

      // The subscribe to real-time updates
      this.http.get(environment.hubUrl).subscribe((response: any) => {
        const options = {
          accessTokenFactory: () => response.accessToken
        }
        const connection = new HubConnectionBuilder()
          .withUrl(response.url, options)
          .build()

        connection.on(environment.hubMethod, (data: Status) => {
          this.status = data
        });

        connection.start();
      });

    });
  }
}

