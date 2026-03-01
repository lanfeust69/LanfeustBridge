import 'zone.js/testing';
import { Component, DebugElement } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { fakeAsync, inject, tick, ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { Score } from '../../score';
import { SuitComponent } from '../suit/suit.component';
import { ScoreFormComponent } from './score-form.component';
import { Suit } from '../../types';

describe('Component: score', () => {
    let fixture: ComponentFixture<ScoreFormComponent>;
    let scoreForm: ScoreFormComponent;
    let element: HTMLElement;
    let debugElement: DebugElement;

    // setup
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [FormsModule],
            declarations: [ScoreFormComponent, SuitComponent]
        });

        vi.useFakeTimers();
        fixture = TestBed.createComponent(ScoreFormComponent);
        scoreForm = fixture.componentInstance;  // to access properties and methods
        element = fixture.nativeElement;        // to access DOM element
        debugElement = fixture.debugElement;    // test helper
    });

    it('should render as a form element with correct elements', async () => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        expect(element.firstChild.nodeName).toBe('FORM');  // uppercase since HTML element
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(0);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.textContent).toBe('Pass');
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(true);
    });

    it('should be valid after clicking "Pass"', async () => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        expect(levels.length).toBe(8);
        const pass = levels[0];
        pass.nativeElement.click();
        fixture.detectChanges();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(0);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(0);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.textContent).toBe('Pass');
        expect(debugElement.query(By.css('button')).nativeElement.disabled).toBe(false);
    });

    it('should not be valid after clicking non-"Pass" level', async () => {
        scoreForm.score = new Score();
        fixture.detectChanges();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        levels[1].nativeElement.click();
        fixture.detectChanges();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(1);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.textContent).toMatch(/^\s*1\s*=\s*$/);
        expect(debugElement.query(By.css('button[type=submit]')).nativeElement.disabled).toBe(true);
    });

    it('should be valid after entering simple score', async () => {
        scoreForm.score = new Score();
        scoreForm.score.vulnerability = 'None';
        fixture.detectChanges();
        const levels = debugElement.queryAll(By.css('input[name=level]'));
        levels[4].nativeElement.click();
        fixture.detectChanges();
        debugElement.queryAll(By.css('input[name=suit]'))[2].nativeElement.click();
        debugElement.queryAll(By.css('input[name=declarer]'))[0].nativeElement.click();
        fixture.detectChanges();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(1);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.textContent).toMatch(/^\s*4\s*.\s*N\s*=\s*\+420$/);
        expect(debugElement.query(By.css('button[type=submit]')).nativeElement.disabled).toBe(false);
    });

    it('should properly display an existing score', async () => {
        const score = new Score();
        score.vulnerability = 'None';
        score.contract = {
            level: 3,
            suit: Suit.Hearts,
            declarer: 'S',
            doubled: false,
            redoubled: false
        };
        score.tricks = 9;
        score.entered = true;
        scoreForm.score = score;
        // not clear why `fixture.detectChanges();` doesn't work as it does in other tests...
        await vi.runAllTimersAsync();
        expect(debugElement.queryAll(By.css('div[name=level]')).length).toBe(1);
        const activeLevelLabel = debugElement.query(By.css('div[name=level]>div:nth-of-type(4)>label'));
        expect(activeLevelLabel).toBeDefined();
        expect(activeLevelLabel.nativeElement.textContent).toMatch(/^\s*3\s*$/);
        const activeLevelInput = debugElement.query(By.css('div[name=level]>div:nth-of-type(4)>input'));
        expect(activeLevelInput).toBeDefined();
        expect(activeLevelInput.nativeElement.checked).toBeTruthy();
        expect(debugElement.queryAll(By.css('div[name=suit]')).length).toBe(1);
        const activeSuit = debugElement.query(By.css('div[name=suit]>div:nth-of-type(3)>input'));
        expect(activeSuit).toBeDefined();
        expect(activeSuit.nativeElement.checked).toBeTruthy();
        expect(debugElement.queryAll(By.css('input[name=doubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('input[name=redoubled]')).length).toBe(1);
        expect(debugElement.queryAll(By.css('div[name=declarer]')).length).toBe(1);
        const activeDeclarerLabel = debugElement.query(By.css('div[name=declarer]>div:nth-of-type(2)>label'));
        expect(activeDeclarerLabel).toBeDefined();
        expect(activeDeclarerLabel.nativeElement.textContent).toMatch(/^\s*S\s*$/);
        const activeDeclarerInpu = debugElement.query(By.css('div[name=declarer]>div:nth-of-type(2)>input'));
        expect(activeDeclarerInpu).toBeDefined();
        expect(activeDeclarerInpu.nativeElement.checked).toBeTruthy();
        expect(debugElement.queryAll(By.css('div[name=result]')).length).toBe(1);
        const display = debugElement.query(By.css('div .score-display'));
        expect(display).toBeDefined();
        expect(display.nativeElement.textContent).toMatch(/^\s*3\s*.\s*S\s*=\s*\+140$/);
        expect(debugElement.query(By.css('button[type=submit]')).nativeElement.disabled).toBe(false);
    });
});
