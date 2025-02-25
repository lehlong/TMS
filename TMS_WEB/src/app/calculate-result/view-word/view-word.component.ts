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
  reportUrl: string = `https://docs.google.com/gview?url=https://docs.google.com/document/d/1hsfxVG47SUBR5GPXYUh1B3Jcbo8_C4mbbmJHmv9mUPk/edit?tab=t.0&embedded=true`

  constructor(
    public sanitizer: DomSanitizer,

  ) { }
  ngOnInit(): void {
    if (this.reportUrl != '') {
      this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.reportUrl);
    }
  }
}
