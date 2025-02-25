import { SafeResourceUrl, DomSanitizer } from "@angular/platform-browser";
import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module'
import { ActivatedRoute } from '@angular/router'
import { environment } from '../../../environments/environment.prod'
import { NzMessageService } from 'ng-zorro-antd/message'

@Component({
  selector: 'app-view-word',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './view-word.component.html',
  styleUrl: './view-word.component.scss'
})
export class ViewWordComponent {

  urlSafe: any;
  reportUrl: string = `https://docs.google.com/gview?url=http://sso.d2s.com.vn/Upload/0bd0716e-76b2-4b97-bf13-8d6222bd5b48.docx&embedded=true`

  constructor(
    public sanitizer: DomSanitizer,

  ) { }
  ngOnInit(): void {
    if (this.reportUrl != '') {
      this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.reportUrl);
    }
  }
}
