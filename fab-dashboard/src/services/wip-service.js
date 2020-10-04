import { fromFetch } from 'rxjs/fetch';
import { map } from 'rxjs/operators';

export  default  class  WipService {

   static url = 'https://eap-webapi.azurewebsites.net/api/wip';
   static getAll$() {
      return fromFetch(this.url, { selector: response => response.json() });
   }
   static get$(eqpId) {
      return fromFetch(`${this.url}/${eqpId}`, { selector: response => response.json() });
   }
   static getDailyStatistics$(eqpId) {
      return fromFetch(`${this.url}/daily-statistics/${eqpId}`, { selector: response => response.json() })
      .pipe(map(o=>{
         o.forEach((val)=>{
            val.date = new Date( val.date ).getTime();
         });
         return o;
      }));
   }
   static getTodayStatistics$(eqpId) {
      return fromFetch(`${this.url}/today-statistics/${eqpId}`, { selector: response => response.json() });
   }
}
