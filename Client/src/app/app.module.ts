import {
  CUSTOM_ELEMENTS_SCHEMA,
  NgModule,
} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BootstrapComponentsModule } from './modules/bootstrap-components.module';
import { SharedModule } from './modules/shared.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { LoginModalComponent } from './components/modals/login-modal/login-modal.component';
import { RegisterModalComponent } from './components/modals/register-modal/register-modal.component';
import { NavPanelComponent } from './components/shared/nav-panel/nav-panel.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginModalComponent,
    RegisterModalComponent,
    NavPanelComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    BootstrapComponentsModule,
    SharedModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
