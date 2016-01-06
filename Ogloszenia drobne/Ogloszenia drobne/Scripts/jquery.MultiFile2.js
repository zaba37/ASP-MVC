/*
 ### jQuery Multiple File Upload Plugin v1.48 - 2012-07-19 ###
 * Home: http://www.fyneworks.com/jquery/multiple-file-upload/
 * Code: http://code.google.com/p/jquery-MultiFile2-plugin/
 *
	* Licensed under http://en.wikipedia.org/wiki/MIT_License
 ###
*/

/*# AVOID COLLISIONS #*/
;if(window.jQuery) (function($){
/*# AVOID COLLISIONS #*/
 
	// plugin initialization
	$.fn.MultiFile2 = function(options){
		if(this.length==0) return this; // quick fail
		
		// Handle API methods
		if(typeof arguments[0]=='string'){
			// Perform API methods on individual elements
			if(this.length>1){
				var args = arguments;
				return this.each(function(){
					$.fn.MultiFile2.apply($(this), args);
    });
			};
			// Invoke API method handler
			$.fn.MultiFile2[arguments[0]].apply(this, $.makeArray(arguments).slice(1) || []);
			// Quick exit...
			return this;
		};
		
		// Initialize options for this call
		var options = $.extend(
			{}/* new object */,
			$.fn.MultiFile2.options/* default options */,
			options || {} /* just-in-time options */
		);
		
		// Empty Element Fix!!!
		// this code will automatically intercept native form submissions
		// and disable empty file elements
		$('form')
		.not('MultiFile2-intercepted')
		.addClass('MultiFile2-intercepted')
		.submit($.fn.MultiFile2.disableEmpty);
		
		//### http://plugins.jquery.com/node/1363
		// utility method to integrate this plugin with others...
		if($.fn.MultiFile2.options.autoIntercept){
			$.fn.MultiFile2.intercept( $.fn.MultiFile2.options.autoIntercept /* array of methods to intercept */ );
			$.fn.MultiFile2.options.autoIntercept = null; /* only run this once */
		};
		
		// loop through each matched element
		this
		 .not('.MultiFile2-applied')
			.addClass('MultiFile2-applied')
		.each(function(){
			//#####################################################################
			// MAIN PLUGIN FUNCTIONALITY - START
			//#####################################################################
			
       // BUG 1251 FIX: http://plugins.jquery.com/project/comments/add/1251
       // variable group_count would repeat itself on multiple calls to the plugin.
       // this would cause a conflict with multiple elements
       // changes scope of variable to global so id will be unique over n calls
       window.MultiFile2 = (window.MultiFile2 || 0) + 1;
       var group_count = window.MultiFile2;
       
       // Copy parent attributes - Thanks to Jonas Wagner
       // we will use this one to create new input elements
       var MultiFile2 = {e:this, E:$(this), clone:$(this).clone()};
       
       //===
       
       //# USE CONFIGURATION
       if(typeof options=='number') options = {max:options};
       var o = $.extend({},
        $.fn.MultiFile2.options,
        options || {},
   					($.metadata? MultiFile2.E.metadata(): ($.meta?MultiFile2.E.data():null)) || {}, /* metadata options */
								{} /* internals */
       );
       // limit number of files that can be selected?
       if(!(o.max>0) /*IsNull(MultiFile2.max)*/){
        o.max = MultiFile2.E.attr('maxlength');
       };
							if(!(o.max>0) /*IsNull(MultiFile2.max)*/){
								o.max = (String(MultiFile2.e.className.match(/\b(max|limit)\-([0-9]+)\b/gi) || ['']).match(/[0-9]+/gi) || [''])[0];
								if(!(o.max>0)) o.max = -1;
								else           o.max = String(o.max).match(/[0-9]+/gi)[0];
							}
       o.max = new Number(o.max);
       // limit extensions?
       o.accept = o.accept || MultiFile2.E.attr('accept') || '';
       if(!o.accept){
        o.accept = (MultiFile2.e.className.match(/\b(accept\-[\w\|]+)\b/gi)) || '';
        o.accept = new String(o.accept).replace(/^(accept|ext)\-/i,'');
       };
       
       //===
       
       // APPLY CONFIGURATION
							$.extend(MultiFile2, o || {});
       MultiFile2.STRING = $.extend({},$.fn.MultiFile2.options.STRING,MultiFile2.STRING);
       
       //===
       
       //#########################################
       // PRIVATE PROPERTIES/METHODS
       $.extend(MultiFile2, {
        n: 0, // How many elements are currently selected?
        slaves: [], files: [],
        instanceKey: MultiFile2.e.id || 'MultiFile2'+String(group_count), // Instance Key?
        generateID: function(z){ return MultiFile2.instanceKey + (z>0 ?'_F'+String(z):''); },
        trigger: function(event, element){
         var handler = MultiFile2[event], value = $(element).attr('value');
         if(handler){
          var returnValue = handler(element, value, MultiFile2);
          if( returnValue!=null ) return returnValue;
         }
         return true;
        }
       });
       
       //===
       
       // Setup dynamic regular expression for extension validation
       // - thanks to John-Paul Bader: http://smyck.de/2006/08/11/javascript-dynamic-regular-expresions/
       if(String(MultiFile2.accept).length>1){
								MultiFile2.accept = MultiFile2.accept.replace(/\W+/g,'|').replace(/^\W|\W$/g,'');
        MultiFile2.rxAccept = new RegExp('\\.('+(MultiFile2.accept?MultiFile2.accept:'')+')$','gi');
       };
       
       //===
       
       // Create wrapper to hold our file list
       MultiFile2.wrapID = MultiFile2.instanceKey+'_wrap'; // Wrapper ID?
       MultiFile2.E.wrap('<div class="MultiFile2-wrap" id="'+MultiFile2.wrapID+'"></div>');
       MultiFile2.wrapper = $('#'+MultiFile2.wrapID+'');
       
       //===
       
       // MultiFile2 MUST have a name - default: file1[], file2[], file3[]
       MultiFile2.e.name = MultiFile2.e.name || 'file'+ group_count +'[]';
       
       //===
       
							if(!MultiFile2.list){
								// Create a wrapper for the list
								// * OPERA BUG: NO_MODIFICATION_ALLOWED_ERR ('list' is a read-only property)
								// this change allows us to keep the files in the order they were selected
								MultiFile2.wrapper.append( '<div class="MultiFile2-list" id="'+MultiFile2.wrapID+'_list"></div>' );
								MultiFile2.list = $('#'+MultiFile2.wrapID+'_list');
							};
       MultiFile2.list = $(MultiFile2.list);
							
       //===
       
       // Bind a new element
       MultiFile2.addSlave = function( slave, slave_count ){
								//if(window.console) console.log('MultiFile2.addSlave',slave_count);
								
        // Keep track of how many elements have been displayed
        MultiFile2.n++;
        // Add reference to master element
        slave.MultiFile2 = MultiFile2;
								
								// BUG FIX: http://plugins.jquery.com/node/1495
								// Clear identifying properties from clones
								if(slave_count>0) slave.id = slave.name = '';
								
        // Define element's ID and name (upload components need this!)
        //slave.id = slave.id || MultiFile2.generateID(slave_count);
								if(slave_count>0) slave.id = MultiFile2.generateID(slave_count);
								//FIX for: http://code.google.com/p/jquery-MultiFile2-plugin/issues/detail?id=23
        
        // 2008-Apr-29: New customizable naming convention (see url below)
        // http://groups.google.com/group/jquery-dev/browse_frm/thread/765c73e41b34f924#
        slave.name = String(MultiFile2.namePattern
         /*master name*/.replace(/\$name/gi,$(MultiFile2.clone).attr('name'))
         /*master id  */.replace(/\$id/gi,  $(MultiFile2.clone).attr('id'))
         /*group count*/.replace(/\$g/gi,   group_count)//(group_count>0?group_count:''))
         /*slave count*/.replace(/\$i/gi,   slave_count)//(slave_count>0?slave_count:''))
        );
        
        // If we've reached maximum number, disable input slave
        if( (MultiFile2.max > 0) && ((MultiFile2.n-1) > (MultiFile2.max)) )//{ // MultiFile2.n Starts at 1, so subtract 1 to find true count
         slave.disabled = true;
        //};
        
        // Remember most recent slave
        MultiFile2.current = MultiFile2.slaves[slave_count] = slave;
        
								// We'll use jQuery from now on
								slave = $(slave);
        
        // Clear value
        slave.val('').attr('value','')[0].value = '';
        
								// Stop plugin initializing on slaves
								slave.addClass('MultiFile2-applied');
								
        // Triggered when a file is selected
        slave.change(function(){
          //if(window.console) console.log('MultiFile2.slave.change',slave_count);
 								 
          // Lose focus to stop IE7 firing onchange again
          $(this).blur();
          
          //# Trigger Event! onFileSelect
          if(!MultiFile2.trigger('onFileSelect', this, MultiFile2)) return false;
          //# End Event!
          
          //# Retrive value of selected file from element
          var ERROR = '', v = String(this.value || ''/*.attr('value)*/);
          
          // check extension
          if(MultiFile2.accept && v && !v.match(MultiFile2.rxAccept))//{
            ERROR = MultiFile2.STRING.denied.replace('$ext', String(v.match(/\.\w{1,4}$/gi)));
           //}
          //};
          
          // Disallow duplicates
										for(var f in MultiFile2.slaves)//{
           if(MultiFile2.slaves[f] && MultiFile2.slaves[f]!=this)//{
  										//console.log(MultiFile2.slaves[f],MultiFile2.slaves[f].value);
            if(MultiFile2.slaves[f].value==v)//{
             ERROR = MultiFile2.STRING.duplicate.replace('$file', v.match(/[^\/\\]+$/gi));
            //};
           //};
          //};
          
          // Create a new file input element
          var newEle = $(MultiFile2.clone).clone();// Copy parent attributes - Thanks to Jonas Wagner
          //# Let's remember which input we've generated so
          // we can disable the empty ones before submission
          // See: http://plugins.jquery.com/node/1495
          newEle.addClass('MultiFile2');
          
          // Handle error
          if(ERROR!=''){
            // Handle error
            MultiFile2.error(ERROR);
												
            // 2007-06-24: BUG FIX - Thanks to Adrian Wróbel <adrian [dot] wrobel [at] gmail.com>
            // Ditch the trouble maker and add a fresh new element
            MultiFile2.n--;
            MultiFile2.addSlave(newEle[0], slave_count);
            slave.parent().prepend(newEle);
            slave.remove();
            return false;
          };
          
          // Hide this element (NB: display:none is evil!)
          $(this).css({ position:'absolute', top: '-3000px' });
          
          // Add new element to the form
          slave.after(newEle);
          
          // Update list
          MultiFile2.addToList( this, slave_count );
          
          // Bind functionality
          MultiFile2.addSlave( newEle[0], slave_count+1 );
          
          //# Trigger Event! afterFileSelect
          if(!MultiFile2.trigger('afterFileSelect', this, MultiFile2)) return false;
          //# End Event!
          
        }); // slave.change()
								
								// Save control to element
								$(slave).data('MultiFile2', MultiFile2);
								
       };// MultiFile2.addSlave
       // Bind a new element
       
       
       
       // Add a new file to the list
       MultiFile2.addToList = function( slave, slave_count ){
        //if(window.console) console.log('MultiFile2.addToList',slave_count);
								
        //# Trigger Event! onFileAppend
        if(!MultiFile2.trigger('onFileAppend', slave, MultiFile2)) return false;
        //# End Event!
        
        // Create label elements
        var
         r = $('<div class="MultiFile2-label"></div>'),
         v = String(slave.value || ''/*.attr('value)*/),
         a = $('<span class="MultiFile2-title" title="'+MultiFile2.STRING.selected.replace('$file', v)+'">'+MultiFile2.STRING.file.replace('$file', v.match(/[^\/\\]+$/gi)[0])+'</span>'),
         b = $('<a class="MultiFile2-remove" href="#'+MultiFile2.wrapID+'">'+MultiFile2.STRING.remove+'</a>');
        
        // Insert label
        MultiFile2.list.append(
         r.append(b, ' ', a)
        );
        
        b
								.click(function(){
         
          //# Trigger Event! onFileRemove
          if(!MultiFile2.trigger('onFileRemove', slave, MultiFile2)) return false;
          //# End Event!
          
          MultiFile2.n--;
          MultiFile2.current.disabled = false;
          
          // Remove element, remove label, point to current
										MultiFile2.slaves[slave_count] = null;
										$(slave).remove();
										$(this).parent().remove();
										
          // Show most current element again (move into view) and clear selection
          $(MultiFile2.current).css({ position:'', top: '' });
										$(MultiFile2.current).reset().val('').attr('value', '')[0].value = '';
          
          //# Trigger Event! afterFileRemove
          if(!MultiFile2.trigger('afterFileRemove', slave, MultiFile2)) return false;
          //# End Event!
										
          return false;
        });
        
        //# Trigger Event! afterFileAppend
        if(!MultiFile2.trigger('afterFileAppend', slave, MultiFile2)) return false;
        //# End Event!
        
       }; // MultiFile2.addToList
       // Add element to selected files list
       
       
       
       // Bind functionality to the first element
       if(!MultiFile2.MultiFile2) MultiFile2.addSlave(MultiFile2.e, 0);
       
       // Increment control count
       //MultiFile2.I++; // using window.MultiFile2
       MultiFile2.n++;
							
							// Save control to element
							MultiFile2.E.data('MultiFile2', MultiFile2);
							

			//#####################################################################
			// MAIN PLUGIN FUNCTIONALITY - END
			//#####################################################################
		}); // each element
	};
	
	/*--------------------------------------------------------*/
	
	/*
		### Core functionality and API ###
	*/
	$.extend($.fn.MultiFile2, {
  /**
   * This method removes all selected files
   *
   * Returns a jQuery collection of all affected elements.
   *
   * @name reset
   * @type jQuery
   * @cat Plugins/MultiFile2
   * @author Diego A. (http://www.fyneworks.com/)
   *
   * @example $.fn.MultiFile2.reset();
   */
  reset: function(){
			var settings = $(this).data('MultiFile2');
			//if(settings) settings.wrapper.find('a.MultiFile2-remove').click();
			if(settings) settings.list.find('a.MultiFile2-remove').click();
   return $(this);
  },
  
  
  /**
   * This utility makes it easy to disable all 'empty' file elements in the document before submitting a form.
   * It marks the affected elements so they can be easily re-enabled after the form submission or validation.
   *
   * Returns a jQuery collection of all affected elements.
   *
   * @name disableEmpty
   * @type jQuery
   * @cat Plugins/MultiFile2
   * @author Diego A. (http://www.fyneworks.com/)
   *
   * @example $.fn.MultiFile2.disableEmpty();
   * @param String class (optional) A string specifying a class to be applied to all affected elements - Default: 'mfD'.
   */
  disableEmpty: function(klass){ klass = (typeof(klass)=='string'?klass:'')||'mfD';
   var o = [];
   $('input:file.MultiFile2').each(function(){ if($(this).val()=='') o[o.length] = this; });
   return $(o).each(function(){ this.disabled = true }).addClass(klass);
  },
  
  
		/**
			* This method re-enables 'empty' file elements that were disabled (and marked) with the $.fn.MultiFile2.disableEmpty method.
			*
			* Returns a jQuery collection of all affected elements.
			*
			* @name reEnableEmpty
			* @type jQuery
			* @cat Plugins/MultiFile2
			* @author Diego A. (http://www.fyneworks.com/)
			*
			* @example $.fn.MultiFile2.reEnableEmpty();
			* @param String klass (optional) A string specifying the class that was used to mark affected elements - Default: 'mfD'.
			*/
  reEnableEmpty: function(klass){ klass = (typeof(klass)=='string'?klass:'')||'mfD';
   return $('input:file.'+klass).removeClass(klass).each(function(){ this.disabled = false });
  },
  
  
		/**
			* This method will intercept other jQuery plugins and disable empty file input elements prior to form submission
			*
	
			* @name intercept
			* @cat Plugins/MultiFile2
			* @author Diego A. (http://www.fyneworks.com/)
			*
			* @example $.fn.MultiFile2.intercept();
			* @param Array methods (optional) Array of method names to be intercepted
			*/
  intercepted: {},
  intercept: function(methods, context, args){
   var method, value; args = args || [];
   if(args.constructor.toString().indexOf("Array")<0) args = [ args ];
   if(typeof(methods)=='function'){
    $.fn.MultiFile2.disableEmpty();
    value = methods.apply(context || window, args);
				//SEE-http://code.google.com/p/jquery-MultiFile2-plugin/issues/detail?id=27
				setTimeout(function(){ $.fn.MultiFile2.reEnableEmpty() },1000);
    return value;
   };
   if(methods.constructor.toString().indexOf("Array")<0) methods = [methods];
   for(var i=0;i<methods.length;i++){
    method = methods[i]+''; // make sure that we have a STRING
    if(method) (function(method){ // make sure that method is ISOLATED for the interception
     $.fn.MultiFile2.intercepted[method] = $.fn[method] || function(){};
     $.fn[method] = function(){
      $.fn.MultiFile2.disableEmpty();
      value = $.fn.MultiFile2.intercepted[method].apply(this, arguments);
						//SEE http://code.google.com/p/jquery-MultiFile2-plugin/issues/detail?id=27
      setTimeout(function(){ $.fn.MultiFile2.reEnableEmpty() },1000);
      return value;
     }; // interception
    })(method); // MAKE SURE THAT method IS ISOLATED for the interception
   };// for each method
  } // $.fn.MultiFile2.intercept
		
 });
	
	/*--------------------------------------------------------*/
	
	/*
		### Default Settings ###
		eg.: You can override default control like this:
		$.fn.MultiFile2.options.accept = 'gif|jpg';
	*/
	$.fn.MultiFile2.options = { //$.extend($.fn.MultiFile2, { options: {
		accept: '', // accepted file extensions
		max: -1,    // maximum number of selectable files
		
		// name to use for newly created elements
		namePattern: '$name', // same name by default (which creates an array)
         /*master name*/ // use $name
         /*master id  */ // use $id
         /*group count*/ // use $g
         /*slave count*/ // use $i
									/*other      */ // use any combination of he above, eg.: $name_file$i
		
		// STRING: collection lets you show messages in different languages
		STRING: {
			remove:'x',
			denied:'You cannot select a $ext file.\nTry again...',
			file:'$file',
			selected:'File selected: $file',
			duplicate:'This file has already been selected:\n$file'
		},
		
		// name of methods that should be automcatically intercepted so the plugin can disable
		// extra file elements that are empty before execution and automatically re-enable them afterwards
  autoIntercept: [ 'submit', 'ajaxSubmit', 'ajaxForm', 'validate', 'valid' /* array of methods to intercept */ ],
		
		// error handling function
		error: function(s){
			/*
			ERROR! blockUI is not currently working in IE
			if($.blockUI){
				$.blockUI({
					message: s.replace(/\n/gi,'<br/>'),
					css: { 
						border:'none', padding:'15px', size:'12.0pt',
						backgroundColor:'#900', color:'#fff',
						opacity:'.8','-webkit-border-radius': '10px','-moz-border-radius': '10px'
					}
				});
				window.setTimeout($.unblockUI, 2000);
			}
			else//{// save a byte!
			*/
			 alert(s);
			//}// save a byte!
		}
 }; //} });
	
	/*--------------------------------------------------------*/
	
	/*
		### Additional Methods ###
		Required functionality outside the plugin's scope
	*/
	
	// Native input reset method - because this alone doesn't always work: $(element).val('').attr('value', '')[0].value = '';
	$.fn.reset = function(){ return this.each(function(){ try{ this.reset(); }catch(e){} }); };
	
	/*--------------------------------------------------------*/
	
	/*
		### Default implementation ###
		The plugin will attach itself to file inputs
		with the class 'multi' when the page loads
	*/
	$(function(){
  //$("input:file.multi").MultiFile2();
  $("input[type=file].multisecond").MultiFile2();
 });
	
	
	
/*# AVOID COLLISIONS #*/
})(jQuery);
/*# AVOID COLLISIONS #*/
