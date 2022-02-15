import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RegisterFields } from 'src/app/models/forms';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register-modal',
  templateUrl: './register-modal.component.html',
  styleUrls: ['./register-modal.component.scss'],
})
export class RegisterModalComponent implements OnInit {
  modalRef?: BsModalRef;
  registerForm!: FormGroup;
  maxDate!: Date;
  validationErrors: string[] = [];

  constructor(
    public accountService: AccountService,
    private formBuilderService: FormBuilder,
    public bsModalRef: BsModalRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeFormGroup();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeFormGroup() {
    this.registerForm = this.formBuilderService.group({
      [RegisterFields.Username]: ['', Validators.required],
      [RegisterFields.Gender]: ['male'],
      [RegisterFields.KnownAs]: ['', Validators.required],
      [RegisterFields.City]: ['', Validators.required],
      [RegisterFields.Country]: ['', Validators.required],
      [RegisterFields.DateOfBirth]: ['', Validators.required],
      [RegisterFields.Password]: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(32),
        ],
      ],
      [RegisterFields.ConfirmPassword]: [
        '',
        [Validators.required, this.matchValues(RegisterFields.Password)],
      ],
    });

    this.registerForm.controls?.[
      RegisterFields.Password
    ].valueChanges.subscribe(() =>
      this.registerForm.controls?.[
        RegisterFields.ConfirmPassword
      ].updateValueAndValidity()
    );
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) =>
      control?.value ===
      (control?.parent?.controls as Record<string, AbstractControl>)?.[matchTo]?.value
        ? null
        : { matching: true };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.close();
      },
      error: (error: string[]) => {
        this.validationErrors = error;
      },
    });
  }

  close() {
    this.bsModalRef.hide();
  }

  switchToRegister() {
    this.accountService.switchModalDialog(this.accountService.registerModalId);
  }

  getUsernameControl() {
    return this.registerForm.controls[RegisterFields.Username] as FormControl;
  }

  getPasswordControl() {
    return this.registerForm.controls[RegisterFields.Username] as FormControl;
  }

  getConfirmPasswordControl() {
    return this.registerForm.controls[
      RegisterFields.ConfirmPassword
    ] as FormControl;
  }

  getCityControl() {
    return this.registerForm.controls[RegisterFields.City] as FormControl;
  }

  getCountryControl() {
    return this.registerForm.controls[RegisterFields.Country] as FormControl;
  }

  getDateOfBirthControl() {
    return this.registerForm.controls[
      RegisterFields.DateOfBirth
    ] as FormControl;
  }

  getGenderControl() {
    return this.registerForm.controls[RegisterFields.Gender] as FormControl;
  }

  getKnownAsControl() {
    return this.registerForm.controls[RegisterFields.KnownAs] as FormControl;
  }
}
