/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PusherService } from './pusher.service';

describe('Service: Pusher', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PusherService]
    });
  });

  it('should ...', inject([PusherService], (service: PusherService) => {
    expect(service).toBeTruthy();
  }));
});
