export interface BasicPageParams {
  page: number;
  pageSize: number;
}

export interface BasicFetchResult<T extends any> {
  items: T[];
  total: number;
}

export interface INameValue<T> {
  name: string;
  value: T;
}

export interface IAvailable {
  isAvailable: boolean;
}

export class Available implements IAvailable {
  isAvailable!: boolean;
}
