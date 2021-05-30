import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from  '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { MatPaginatorModule } from '@angular/material/paginator';
import { DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterLostDogComponent } from './Components/register-lost-dog/register-lost-dog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { HeaderComponent } from './Components/header/header.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { FooterComponent } from './Components/footer/footer.component';

import { ErrorInterceptor } from './helpers/error-interceptor';
import { JwtInterceptor } from './helpers/jwt-interceptor';
import { RegisterUserComponent } from './Components/register-user/register-user.component';
import { EditLostDogComponent } from './Components/edit-lost-dog/edit-lost-dog.component';
import { EditContactInfoComponent } from './Components/edit-contact-info/edit-contact-info.component';
import { FilterLostDogsComponent } from './Components/filter-lost-dogs/filter-lost-dogs.component';
import { ShelterEmployeeHomePageComponent } from './Components/shelter-employee-home-page/shelter-employee-home-page.component';
import { ShelterEmployeeNavbarComponent } from './Components/shelter-employee-navbar/shelter-employee-navbar.component';
import { SeeSheltersListComponent } from './Components/see-shelters-list/see-shelters-list.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterLostDogComponent,
    HomePageComponent,
    HeaderComponent,
    NavbarComponent,
    FooterComponent,
    RegisterUserComponent,
    EditLostDogComponent,
    EditContactInfoComponent,
    FilterLostDogsComponent,
    ShelterEmployeeHomePageComponent,
    ShelterEmployeeNavbarComponent,
    SeeSheltersListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatGridListModule,
    MatListModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatPaginatorModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    DatePipe
  ],
  bootstrap: [AppComponent],
  exports: [
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatGridListModule,
    MatListModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatPaginatorModule,
  ]
})
export class AppModule { }
