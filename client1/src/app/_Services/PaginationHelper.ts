import { HttpClient, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/Member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

export function getPaginatedresult<T>(url, params, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
 
     return http
       .get<T>(url, { observe: 'response', params })
       .pipe(
         map(response => {
           paginatedResult.result = response.body;
           if (response.headers.get('Pagination') !== null) {
           paginatedResult.pagination = JSON.parse(
               response.headers.get('Pagination')
             );
           }
           return paginatedResult;
         })
       );
   }
 
   export function getPaginationHeaders(pageNumber: number, pageSize: number){
 
     let params = new HttpParams();
   
       params = params.append('pageNumber', pageNumber.toString());
       params = params.append('pageSize', pageSize.toString());
  
     return params;
   } 
 
  