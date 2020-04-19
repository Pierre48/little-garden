import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgImageSliderModule } from 'ng-image-slider';
import { JwPaginationComponent } from 'jw-angular-pagination';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SeedlingListComponent } from './seedling-list/seedling-list.component';
import { SeedlingDetailComponent } from './seedling-detail/seedling-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SeedlingListComponent,
    SeedlingDetailComponent,
    JwPaginationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    NgImageSliderModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SeedlingListComponent, pathMatch: 'full' },
      { path: 'seedlingdetail/:id', component: SeedlingDetailComponent },
      { path: 'seedlinglist', component: SeedlingListComponent },
      { path: '**', component: SeedlingListComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
