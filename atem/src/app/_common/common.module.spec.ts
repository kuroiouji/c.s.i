import { CtmModule } from './common.module';

describe('CtmModule', () => {
  let commonModule: CtmModule;

  beforeEach(() => {
    commonModule = new CtmModule();
  });

  it('should create an instance', () => {
    expect(commonModule).toBeTruthy();
  });
});
