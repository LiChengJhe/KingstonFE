import { fromFetch } from 'rxjs/fetch';
export default class EqpService {

   static url = 'https://eap-webapi.azurewebsites.net/api/eqp';

   static getAll$() {
      return fromFetch(this.url, { selector: response => response.json() });
   }
}
