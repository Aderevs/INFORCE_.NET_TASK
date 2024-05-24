import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllComponent } from './components/all/all.component';
import { CertainComponent } from './components/certain/certain.component';
import { RouterModule } from '@angular/router';




@NgModule({
  declarations: [
    AllComponent,
    CertainComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ]
})
export class ShortenedUrlsModule { }
