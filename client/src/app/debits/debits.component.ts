import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-debit',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    CommonModule,
    MatSelectModule,
    MatDatepickerModule,
  ],
  templateUrl: './debits.component.html',
  styleUrls: ['./debits.component.css'],
})
export class DebitsComponent {
  debitForm: FormGroup;
  categories = [
    { id: 1, name: 'Groceries' },
    { id: 2, name: 'Utilities' },
    { id: 3, name: 'Entertainment' },
  ];
  selectedCategory: number | null = null;
  customCategory: string = ''; // Custom category input by the user
  amount: number | null = null; // Amount entered by the user
  date: string = ''; // Date selected by the user (in yyyy-mm-dd format)

  descriptionArray: string[] = [
    'Rent Payment',
    'Grocery Shopping',
    'Electric Bill',
    'Entertainment Expense',
    'Dining Out',
  ];

  // Selected description from the dropdown
  selectedDescription: string = '';

  // Custom description input by the user
  customDescription: string = '';

  constructor(private fb: FormBuilder) {
    this.debitForm = this.fb.group({
      category: ['', Validators.required],
      description: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      date: ['', Validators.required],
      account: ['', Validators.required],
    });
  }

  onSubmit() {
    // Get the category to submit (either selected or custom)
    const categoryToSubmit = this.selectedCategory
      ? this.selectedCategory
      : this.customCategory;
    const descriptionToSubmit =
      this.selectedDescription || this.customDescription;

    console.log('Submitting with category:', categoryToSubmit);
    console.log('Submitting with description:', descriptionToSubmit);
    console.log('Submitting with amount:', this.amount);
    console.log('Submitting with date:', this.date);
    // You can now send this data to the backend or handle it accordingly
  }
}
