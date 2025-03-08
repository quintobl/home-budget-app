import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
import { ToastrService } from 'ngx-toastr';
import { Category } from '../_models/category';
import { Account } from '../_models/account';
import { catchError, of } from 'rxjs';

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
  categories: Category[] = [];
  accounts: Account[] = [];
  descriptions: any[] = [];
  filteredDescriptions: any[] = [];

  constructor(
    private fb: FormBuilder,
    private debitService: DebitService,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {
    this.debitForm = this.fb.group({
      accountId: [null, Validators.required],
      categoryId: [null],
      customCategory: [''],
      descriptionId: [null],
      customDescription: [''],
      amount: [null, [Validators.required, Validators.min(0.01)]],
      date: [null, Validators.required],
    });
  }

  public ngOnInit(): void {
    this.loadCategories();
    this.loadAccounts();
  }

  public loadAccounts(): void {
    this.debitService.getAccounts().subscribe((data) => {
      this.accounts = data;
    });
  }

  public loadCategories(): void {
    this.debitService.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  public loadDescriptions(categoryId: number): void {
    if (!categoryId) {
      this.filteredDescriptions = [];
      return;
    }

    this.debitService
      .getDescriptionsByCategory(categoryId)
      .pipe(
        catchError(() => {
          this.toastr.error('An error occurred loading descriptions.');
          return of([]);
        })
      )
      .subscribe((data) => (this.filteredDescriptions = data));
  }

  public onCategoryChange(event: any): void {
    const selectedCategoryId = Number(event.target.value);
    this.loadDescriptions(selectedCategoryId);

    if (!selectedCategoryId || selectedCategoryId === 0) {
      this.filteredDescriptions = [];
      this.debitForm.get('description')?.disable();
      return;
    }

    this.debitService
      .getDescriptionsByCategory(selectedCategoryId)
      .pipe(
        catchError(() => {
          this.toastr.error('An error occurred loading descriptions.');
          return of([]);
        })
      )
      .subscribe((data) => (this.filteredDescriptions = data));
  }

  public onDescriptionChange(event: any): void {
    const selectedDescriptionId = Number(event.target.value);
    this.debitForm.get('description')?.setValue(selectedDescriptionId);
  }

  public onSubmit(): void {
    if (this.debitForm.invalid) {
      debugger;
      this.toastr.error('There was an error saving the debit!');
      return;
    } else {
      const debitData = this.debitForm.value;

      if (this.filteredDescriptions.length > 0) {
        debitData.descriptionId = this.filteredDescriptions[0]?.id;

        debitData.accountId = debitData.accountId
          ? Number(debitData.accountId)
          : null;
        debitData.categoryId = debitData.categoryId
          ? Number(debitData.categoryId)
          : null;
        debitData.descriptionId = debitData.descriptionId
          ? Number(debitData.descriptionId)
          : null;

        const selectedAccount = this.accounts.find(
          (a) => a.id === debitData.accountId
        );
        debitData.accountName = selectedAccount ? selectedAccount.name : null;

        const selectedCategory = this.categories.find(
          (c) => c.id === debitData.categoryId
        );
        debitData.categoryName = selectedCategory
          ? selectedCategory.name
          : null;

        const selectedDescription = this.filteredDescriptions.find(
          (d) => d.id === debitData.descriptionId
        );
        debitData.descriptionName = selectedDescription
          ? selectedDescription.name
          : null;
      }

      if (debitData.customCategory) {
        this.debitService.addCategory(debitData.customCategory).subscribe(
          (newCategory) => {
            console.log('New Category Response:', newCategory);
            if (newCategory && newCategory.id) {
              debitData.categoryId = newCategory.id;
              debitData.categoryName = newCategory.name;
            } else {
              console.error('Category ID is missing from API response!');
            }
            console.log('Updated debitData before description:', debitData);
            this.cdr.detectChanges();
            this.processDebitSubmission(debitData);
            this.loadCategories();
          },
          (error) => {
            console.error('Error adding category:', error);
          }
        );
      } else {
        this.processDebitSubmission(debitData);
      }
    }
  }

  private processDebitSubmission(debitData: any): void {
    console.log(
      'Processing debit submission, categoryId:',
      debitData.categoryId
    );
    if (debitData.customDescription) {
      this.debitService
        .addDescription(debitData.customDescription, debitData.categoryId || 0)
        .subscribe(
          (newDescription) => {
            if (newDescription && newDescription.id) {
              debitData.descriptionId = newDescription.id;
              debitData.descriptionName = newDescription.name;
            } else {
              console.error('Description ID is missing from API response!');
            }
            console.log('Updated debitData:', debitData);
            this.submitDebit(debitData);
            this.loadDescriptions(debitData.categoryId || 0);
          },
          (error) => {
            console.error('Error adding description:', error);
          }
        );
    } else {
      this.submitDebit(debitData);
    }
  }

  private submitDebit(debitData: any): void {
    debitData.date = new Date(debitData.date).toISOString();
    this.debitService.saveDebit(debitData).subscribe(
      (response) => {
        console.log('Debit saved:', response);
        this.toastr.success('Debit Saved!');
        this.debitForm.reset();
        this.filteredDescriptions = [];
      },
      (error) => {
        console.error('Error saving debit:', error);
      }
    );
  }
}
