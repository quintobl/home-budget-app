import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../_models/category';
import { Account } from '../_models/account';
import { catchError, of } from 'rxjs';
import { CreditService } from '../_services/credit.service';

@Component({
  selector: 'app-credits',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
  ],
  templateUrl: './credits.component.html',
  styleUrl: './credits.component.css',
})
export class CreditsComponent implements OnInit {
  creditForm: FormGroup;
  categories: Category[] = [];
  accounts: Account[] = [];
  descriptions: any[] = [];
  filteredDescriptions: any[] = [];

  constructor(
    private fb: FormBuilder,
    private creditService: CreditService,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {
    this.creditForm = this.fb.group({
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
    this.creditService.getAccounts().subscribe((data) => {
      this.accounts = data;
    });
  }

  public loadCategories(): void {
    this.creditService.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  public loadDescriptions(categoryId: number): void {
    if (!categoryId) {
      this.filteredDescriptions = [];
      return;
    }

    this.creditService
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
      this.creditForm.get('description')?.disable();
      return;
    }

    this.creditService
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
    this.creditForm.get('description')?.setValue(selectedDescriptionId);
  }

  public onSubmit(): void {
    if (this.creditForm.invalid) {
      debugger;
      this.toastr.error('There was an error saving the credit!');
      return;
    } else {
      const creditData = this.creditForm.value;

      if (this.filteredDescriptions.length > 0) {
        creditData.descriptionId = this.filteredDescriptions[0]?.id;

        creditData.accountId = creditData.accountId
          ? Number(creditData.accountId)
          : null;
        creditData.categoryId = creditData.categoryId
          ? Number(creditData.categoryId)
          : null;
        creditData.descriptionId = creditData.descriptionId
          ? Number(creditData.descriptionId)
          : null;

        const selectedAccount = this.accounts.find(
          (a) => a.id === creditData.accountId
        );
        creditData.accountName = selectedAccount ? selectedAccount.name : null;

        const selectedCategory = this.categories.find(
          (c) => c.id === creditData.categoryId
        );
        creditData.categoryName = selectedCategory
          ? selectedCategory.name
          : null;

        const selectedDescription = this.filteredDescriptions.find(
          (d) => d.id === creditData.descriptionId
        );
        creditData.descriptionName = selectedDescription
          ? selectedDescription.name
          : null;
      }

      if (creditData.customCategory) {
        this.creditService.addCategory(creditData.customCategory).subscribe(
          (newCategory) => {
            console.log('New Category Response:', newCategory);
            if (newCategory && newCategory.id) {
              creditData.categoryId = newCategory.id;
              creditData.categoryName = newCategory.name;
            } else {
              console.error('Category ID is missing from API response!');
            }
            console.log('Updated creditData before description:', creditData);
            this.cdr.detectChanges();
            this.processCreditSubmission(creditData);
            this.loadCategories();
          },
          (error) => {
            console.error('Error adding category:', error);
          }
        );
      } else {
        this.processCreditSubmission(creditData);
      }
    }
  }

  private processCreditSubmission(creditData: any): void {
    console.log(
      'Processing credit submission, categoryId:',
      creditData.categoryId
    );
    if (creditData.customDescription) {
      this.creditService
        .addDescription(
          creditData.customDescription,
          creditData.categoryId || 0
        )
        .subscribe(
          (newDescription) => {
            if (newDescription && newDescription.id) {
              creditData.descriptionId = newDescription.id;
              creditData.descriptionName = newDescription.name;
            } else {
              console.error('Description ID is missing from API response!');
            }
            console.log('Updated creditData:', creditData);
            this.submitCredit(creditData);
            this.loadDescriptions(creditData.categoryId || 0);
          },
          (error) => {
            console.error('Error adding description:', error);
          }
        );
    } else {
      this.submitCredit(creditData);
    }
  }

  private submitCredit(creditData: any): void {
    creditData.date = new Date(creditData.date).toISOString();
    this.creditService.saveCredit(creditData).subscribe(
      (response) => {
        console.log('Credit saved:', response);
        this.toastr.success('Credit Saved!');
        this.creditForm.reset();
        this.filteredDescriptions = [];
      },
      (error) => {
        console.error('Error saving credit:', error);
      }
    );
  }
}
