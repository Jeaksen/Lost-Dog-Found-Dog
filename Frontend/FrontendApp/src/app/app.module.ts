import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterLostDogComponent } from './Components/register-lost-dog/register-lost-dog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterLostDogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
