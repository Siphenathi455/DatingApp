import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_Services/admin.service';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions, } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  photos: Photo[] =[];

  constructor(private admin : AdminService) { }
 

  ngOnInit(): void {
  this.GetPhotosForApproval();
    
  }

  GetPhotosForApproval()  {
    this.admin.GetPhotosForApproval().subscribe(photo =>{
      this.photos = photo; })

  }
   ApprovePhoto(photoId: number){
    this.admin.ApprovePhoto(photoId).subscribe(() => {
      this.photos = this.photos.filter(x => x.id !== photoId);
    });
  }

  RejectPhoto(photoId: number){
    this.admin.RejectPhoto(photoId)
    .subscribe(() => {
      this.photos = this.photos.filter(x => x.id !== photoId);
    });
  }

}
