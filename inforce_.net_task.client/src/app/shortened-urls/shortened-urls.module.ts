import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllComponent } from './components/all/all.component';
import { CertainComponent } from './components/certain/certain.component';



@NgModule({
  declarations: [
    AllComponent,
    CertainComponent
  ],
  imports: [
    CommonModule
  ]
})
export class ShortenedUrlsModule { }
