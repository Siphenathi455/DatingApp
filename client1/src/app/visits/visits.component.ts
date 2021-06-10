import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/Member';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_Services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class VisitsComponent implements OnInit {
  members: Partial<Member[]>;
  predicate = 'visited';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadVisits();
  }

  loadVisits() {
    this.memberService.getVisits(this.predicate, this.pageNumber, this.pageSize).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;

    })

  }

  pageChange(event: any){
    this.pageNumber = event.page;
    this.loadVisits();
  }

}
