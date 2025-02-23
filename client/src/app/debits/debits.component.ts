import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DebitService } from '../_services/debit.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-debit',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
  ],
  templateUrl: './debits.component.html',
  styleUrls: ['./debits.component.css'],
})
export class DebitsComponent implements OnInit {
  debitForm: FormGroup;
  categories: any[] = [];
  accounts: any[] = [];
  descriptions: { [key: number]: string[] } = {};
  filteredDescriptions: string[] = [];

  constructor(private fb: FormBuilder, private debitService: DebitService) {
    this.debitForm = this.fb.group({
      account: ['', Validators.required],
      category: [''],
      customCategory: [''],
      description: [''],
      customDescription: [''],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      date: ['', Validators.required],
    });
  }

  public ngOnInit(): void {
    console.log(this.debitForm.valid, this.debitForm.value);
    this.loadCategories();
    this.loadDescriptions();
    this.loadAccounts();
  }

  public loadCategories(): void {
    this.debitService.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  public loadDescriptions(): void {
    this.debitService.getDescriptions().subscribe((data) => {
      this.descriptions = data;
    });
  }

  public loadAccounts(): void {
    this.debitService.getAccounts().subscribe((data) => {
      this.accounts = data;
    });
  }

  public onCategoryChange(event: any): void {
    const selectedCategoryId = event.target.value;
    this.filteredDescriptions = this.descriptions[selectedCategoryId] || [];
  }

  public onSubmit(): void {
    if (this.debitForm.invalid) return;

    debugger;

    const debitData = this.debitForm.value;

    debitData.CategoryName = debitData.customCategory || debitData.category;

    debitData.DescriptionId = debitData.description;

    if (debitData.customDescription) {
      debitData.DescriptionId = -1;
    }

    debitData.date = new Date(debitData.date).toISOString();

    console.log('Submitting debit:', JSON.stringify(debitData));

    this.debitService.saveDebit(debitData).subscribe(
      (response) => {
        console.log('Debit saved:', response);
      },
      (error) => {
        console.error('Error saving debit:', error);
      }
    );
  }
}
