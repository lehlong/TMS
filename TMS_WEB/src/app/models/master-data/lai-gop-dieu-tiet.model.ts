import { BaseFilter } from '../base.model'
export class LaiGopDieuTietFilter extends BaseFilter {
  code: string = ''
  name: string = ''
  isActive?: boolean | string | null
  SortColumn: string = ''
  IsDescending: boolean = true
}
