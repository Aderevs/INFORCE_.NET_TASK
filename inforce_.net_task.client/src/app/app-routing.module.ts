import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { accountRoutes } from './account/routes';
import { shortenedUrlsRoutes } from './shortened-urls/routes';

const routes: Routes = [
  ...accountRoutes,
  ...shortenedUrlsRoutes
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
