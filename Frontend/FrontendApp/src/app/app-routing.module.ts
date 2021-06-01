import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterLostDogComponent } from './Components/register-lost-dog/register-lost-dog.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterUserComponent } from './Components/register-user/register-user.component';
import { EditLostDogComponent } from './Components/edit-lost-dog/edit-lost-dog.component';
import { EditContactInfoComponent } from './Components/edit-contact-info/edit-contact-info.component';
import { FilterLostDogsComponent } from './Components/filter-lost-dogs/filter-lost-dogs.component';
import { SeeSheltersListComponent } from './Components/see-shelters-list/see-shelters-list.component';
import { ShelterEmployeeHomePageComponent } from './Components/shelter-employee-home-page/shelter-employee-home-page.component';
import { ShelterDetailsComponent } from './Components/shelter-details/shelter-details.component';
import { LostDogDetailsComponent } from './Components/lost-dog-details/lost-dog-details.component';
import { RegisterShelterDogComponent } from './Components/register-shelter-dog/register-shelter-dog.component';
import { ShelterDogDetailsComponent } from './Components/shelter-dog-details/shelter-dog-details.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'register', component: RegisterUserComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'register-lost-dog', component: RegisterLostDogComponent },
  { path: 'edit-lost-dog/:dogId', component: EditLostDogComponent },
  { path: 'edit-contact-info', component: EditContactInfoComponent },
  { path: 'search', component: FilterLostDogsComponent },
  { path: 'shelters-list', component: SeeSheltersListComponent },
  { path: 'shelter-employee-home', component: ShelterEmployeeHomePageComponent },
  { path: 'lost-dog-details/:dogId', component: LostDogDetailsComponent },
  { path: 'shelter-details/:shelterId', component: ShelterDetailsComponent },
  { path: 'register-shelter-dog/:shelterId', component: RegisterShelterDogComponent },
  { path: 'shelter-dog-details/:shelterId/:dogId', component: ShelterDogDetailsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
