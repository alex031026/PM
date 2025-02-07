import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterStep1Component } from './components/register.step1/register.step1.component';
import { RegisterStep2Component } from './components/register.step2/register.step2.component';

const routes: Routes = [
  { path: '', component: RegisterStep1Component },
  { path: 'step2', component: RegisterStep2Component }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
