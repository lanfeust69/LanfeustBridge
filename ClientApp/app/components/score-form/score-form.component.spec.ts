import { Component, DebugElement } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { async, fakeAsync, inject, tick, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { Score } from '../../score';
import { SuitComponent } from '../suit/suit.component';
import { ScoreFormComponent } from './score-form.component';

describe('Component: score', () => {
    let fixture: ComponentFixture<ScoreFormComponent>;
    let scoreForm: ScoreFormComponent;
    let element: HTMLElement;
    let debugElement: DebugElement;

    //setup
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [FormsModule],
            declarations: [ScoreFormComponent, SuitComponent]
        });

        fixture = TestBed.createComponent(ScoreFormComponent);
        scoreForm = fixture.componentInstance;  // to access properties and methods
        element = fixture.nativeElement;        // to access DOM element
        debugElement = fixture.debugElement;    // test helper
    });

    it('should render as a form element with correct elements', fakeAsync(() => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        tick();
        expect(element.firstChild.nodeName).toBe('FORM');  // uppercase since HTML element
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(0);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.innerText).toBe('Pass');
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(true);
    }));

    it('should be valid after clicking "Pass"', fakeAsync(() => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        tick();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        expect(levels.length).toBe(8);
        const pass = levels[0];
        pass.nativeElement.click();
        fixture.detectChanges();
        tick();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(0);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.innerText).toBe('Pass');
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(false);
    }));

    it('should not be valid after clicking non-"Pass" level', fakeAsync(() => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        tick();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        levels[1].nativeElement.click();
        fixture.detectChanges();
        tick();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(1);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.innerText).toMatch(/^1\s*=\s*$/);
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(true);
    }));

    it('should be valid after entering simple score', fakeAsync(() => {
        scoreForm.score = new Score();
        scoreForm.score.vulnerability = 'None';
        fixture.detectChanges();
        tick();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        levels[4].nativeElement.click();
        fixture.detectChanges();
        tick();
        debugElement.queryAll(By.css('input[name=suit]'))[2].nativeElement.click();
        debugElement.queryAll(By.css('input[name=declarer]'))[0].nativeElement.click();
        fixture.detectChanges();
        tick();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(1);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.innerText).toMatch(/^4\s*.\s*N\s*=\s*\+420$/);
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(false);
    }));
}) 