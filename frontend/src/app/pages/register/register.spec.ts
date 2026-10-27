import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter, Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { Register } from './register';

describe('Register', () => {
  let fixture: ComponentFixture<Register>;
  let http: HttpTestingController;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Register],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    http = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  it('submits account details and navigates to sign in on success', () => {
    const navigate = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    fixture.componentInstance.register('Alex Rivera', 'alex@example.com', 'secret');
    const request = http.expectOne(`${environment.apiBaseUrl}/api/auth/register`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({
      fullName: 'Alex Rivera',
      email: 'alex@example.com',
      password: 'secret',
    });
    expect(fixture.componentInstance.loading()).toBe(true);

    request.flush({});
    expect(fixture.componentInstance.loading()).toBe(false);
    expect(navigate).toHaveBeenCalledWith('/login');
  });

  it('shows the server error and clears loading after a failed request', () => {
    fixture.componentInstance.register('Alex Rivera', 'alex@example.com', 'secret');
    http.expectOne(`${environment.apiBaseUrl}/api/auth/register`).flush(
      { message: 'Email is already registered' },
      { status: 409, statusText: 'Conflict' },
    );

    fixture.detectChanges();
    expect(fixture.componentInstance.loading()).toBe(false);
    expect(fixture.nativeElement.querySelector('[role="alert"]')?.textContent).toContain(
      'Email is already registered',
    );
  });
});
