import { Component, OnInit } from '@angular/core';
import { LoadingService } from './services/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private spinner: LoadingService) {}

  ngOnInit(): void {
    this.spinner.loading();

    setTimeout(() => {
      this.spinner.idle();
    }, 1000);
  }
  title = 'Client';
}
