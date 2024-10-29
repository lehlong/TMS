import { BaseFilter } from '../base.model'
export class CalculateResultListFilter extends BaseFilter {
  code: string = ''
  name: string = ''

  isActive?: boolean | string | null
  SortColumn: string = ''
  IsDescending: boolean = true
}
