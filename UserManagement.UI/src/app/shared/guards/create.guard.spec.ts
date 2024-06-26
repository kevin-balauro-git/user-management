import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { createGuard } from './create.guard';

describe('createGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => createGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
