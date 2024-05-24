import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-certain',
  templateUrl: './certain.component.html',
  styleUrl: './certain.component.css'
})
export class CertainComponent implements OnInit {
  id!: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      // Тепер this.id містить значення id з URL
    });
  }
}
