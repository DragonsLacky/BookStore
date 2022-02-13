import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeagoModule } from 'ngx-timeago';
// import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    // NgxSpinnerModule,
    NgxGalleryModule,
    FileUploadModule,
    TimeagoModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
  ],
  exports: [
    // NgxSpinnerModule,
    TimeagoModule,
    NgxGalleryModule,
    FileUploadModule,
    ToastrModule,
  ],
})
export class SharedModule {}
