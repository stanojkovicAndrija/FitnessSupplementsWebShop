import { HttpClient, HttpHeaders } from '@angular/common/http';
import { nullSafeIsEquivalent } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject} from 'rxjs';
import { environment } from 'src/environments/environment';
import { map} from 'rxjs/operators';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  
  baseUrl = environment.apiUrl;
  private currentUserSource= new BehaviorSubject<IUser>(null as any);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,private router: Router) { }

  loadCurrentUser(token: string){
    let headers= new HttpHeaders();
    headers= headers.set('Authorization',`Bearer ${token}`);
  }

  login(values: any)
  {
    return this.http.post(this.baseUrl+"auth/login",values).pipe(
      map((user: any) => {
        if(user)
        {
          localStorage.setItem('token',user.token);
          this.currentUserSource.next(user);
        }
      })
    )
  }
  logout()
  {
    localStorage.removeItem('token');
    this.currentUserSource.next(null as any);
    this.router.navigateByUrl('/')
  }
  //checkEmailExists(email:string){
    //return this.http.get(this.baseUrl+'/account/emailexists?email='+email);
  //}
}
