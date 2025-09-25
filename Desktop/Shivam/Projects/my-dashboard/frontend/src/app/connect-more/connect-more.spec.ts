import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConnectMore } from './connect-more';

describe('ConnectMore', () => {
  let component: ConnectMore;
  let fixture: ComponentFixture<ConnectMore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConnectMore]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConnectMore);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
