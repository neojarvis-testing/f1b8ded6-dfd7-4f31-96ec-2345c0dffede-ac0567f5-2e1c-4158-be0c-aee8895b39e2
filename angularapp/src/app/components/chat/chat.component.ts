import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  public messages: { user: string, message: string }[] = [];
  public newMessage: string = '';
  public userName: string = ''; // Replace with actual user data as needed
  public connectionEstablished: boolean = false;
  public showEmojiPanel: boolean = false; // Controls display of the emoji picker panel
  private hubConnection!: signalR.HubConnection;
  role:string;

  constructor(private cdr: ChangeDetectorRef) {} // Inject ChangeDetectorRef

  ngOnInit(): void {
    this.role = localStorage.getItem('userRole')
    this.startConnection();
    this.registerOnServerEvents();
    this.userName = localStorage.getItem('userName') || '';
  }

  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(
        'https://8080-aabcfacbacdffcbffdbecdcbacfecbecaeebe.premiumproject.examly.io/chatHub',
        {
          transport: signalR.HttpTransportType.LongPolling,
          accessTokenFactory: () => localStorage.getItem('token') || ''
        }
      )
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established.');
        this.connectionEstablished = true;
      })
      .catch(err => {
        console.error('Error while establishing connection:', err);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      console.log('Received message:', user, message);
      this.messages.push({ user, message });
      this.cdr.detectChanges(); // Trigger change detection to refresh the view
    });
  }

  public sendMessage(): void {
    if (!this.connectionEstablished) {
      console.error('Cannot send message: SignalR connection is not established yet.');
      return;
    }
    
    if (this.newMessage.trim() !== '') {
      this.hubConnection.invoke('SendMessage', this.userName, this.newMessage)
        .then(() => {
          console.log('Message sent successfully.');
          this.newMessage = ''; // Clear the message box after sending
        })
        .catch(err => console.error('Error sending message:', err));
    }
  }

  // Toggles the display of the emoji panel.
  public toggleEmojiPanel(): void {
    this.showEmojiPanel = !this.showEmojiPanel;
  }

  // Appends the selected emoji to the message input.
  public selectEmoji(emoji: string): void {
    this.newMessage += emoji;
  }
}
