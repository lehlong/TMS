import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { MgListTablesGroupsFilter } from '../../models/master-data/mg-list-tables-groups.model';
import { PaginationResult } from '../../models/base.model';
import { MGLISTTABLE_RIGHTS } from '../../shared/constants';
import { MgListTablesGroupsGroupsService } from '../../services/master-data/mg-list-tables-groups.service';
import { GlobalService } from '../../services/global.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ActivatedRoute, Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-mg-list-tables-groups',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './mg-list-tables-groups.component.html',
  styleUrl: './mg-list-tables-groups.component.scss'
})
export class MgListTablesGroupsComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    id: ['', [Validators.required]],
    name: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new MgListTablesGroupsFilter()
  paginationResult = new PaginationResult()
  mgListTablesGroups: any[] = []
  loading: boolean = false
  MGLISTTABLE_RIGHTS = MGLISTTABLE_RIGHTS
  constructor(
    private _service: MgListTablesGroupsGroupsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách bảng biểu',
        path: 'master-data/mg-list-tables-groups',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search()
  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  search() {
    this.isSubmit = false
    this._service.searchMgListTablesGroups(this.filter).subscribe({
      next: (data) => {
        this.mgListTablesGroups = data.data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }



  isCodeExist(id: string): boolean {
    return this.mgListTablesGroups?.some(
      (groups: any) => groups.id === id,
    )
  }

  submitForm(): void {
    this.isSubmit = true
    if (this.validateForm.valid) {
      const formData = this.validateForm.getRawValue()
      if (this.edit) {
        this._service.updateMgListTablesGroups(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      } else {
        if (this.isCodeExist(formData.id)) {
          this.message.error(
            `Mã loại bảng biểu ${formData.id} đã tồn tại, vui lòng nhập lại`,
          )
          return
        }
        formData.code = uuidv4();
        this._service.createMgListTablesGroups(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      }
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty()
          control.updateValueAndValidity({ onlySelf: true })
        }
      })
    }
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new MgListTablesGroupsFilter()
    this.search()
  }

  openCreate() {
    this.edit = false
    this.visible = true
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string) {
    this._service.deleteMgListTablesGroups(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  openEdit(data: {
    code: string
    id: string
    name: string
    isActive: boolean
  }) {
    this.validateForm.setValue({
      code: data.code,
      id: data.id,
      name: data.name,
      isActive: data.isActive,
    })
    setTimeout(() => {
      this.edit = true
      this.visible = true
    }, 200)
  }

  preview(data: { code: string }) {
    console.log(data);

    this._service.searchMgListTablesGroups({ KeyWord: data.code }).subscribe({
      next: (result) => {
        if (result.data && result.data.length > 0) {
          const mgListTableData = result.data[0]
          this.router.navigate(['/master-data/mg-list-tables', data.code], {
            state: { mgListTableData },
          })
        }
      },
      error: (error) => console.error(error),
    })
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }

}
