import { HubConnectionBuilder } from '@microsoft/signalr';
import { from , Subject } from 'rxjs';

export default class DashboardHub {
  hubConn;
  isConnected;
  dataSubject;
  constructor() {
    this.dataSubject = new Subject();
    this.hubConn = new HubConnectionBuilder()
      .withUrl('https://eap-webapi.azurewebsites.net/dashboard-hub')
      .withAutomaticReconnect()
      .build();
  }

  connect(callback) {
    if (!this.isConnected) {
      from(this.hubConn.start()).subscribe(
        (res) => {
          this.isConnected = true;
          callback(this.isConnected);
          console.log('WebSocket connection started !');
        },
        (err) => {
          this.isConnected = false;
          callback(this.isConnected);
          console.log('Error while establishing connection :(');
        });
    }
  }
  close(){
    this.hubConn.stop();
  }
  onReconnect(callback) {
    this.hubConn.onreconnected(callback);
  }

  onReconnecting(callback) {
    this.hubConn.onreconnecting(callback);
  }

  onClose(callback) {
    this.hubConn.onclose(callback);
  }

  joinGroup(groupId='group-1'){
    return from(this.hubConn.invoke('JoinGroup', groupId));
  }

  exitGroup(groupId='group-1') {
    return from(this.hubConn.invoke('ExitGroup', groupId));
  }

  enableReceiver() {
    this.hubConn.on('ReplyEqpInfo', (eqp) => { this.dataSubject.next(eqp); });
  }
  disableReceiver(){
    this.hubConn.off('ReplyEqpInfo');
  }
  getData$() {
    return this.dataSubject;
  }
}
