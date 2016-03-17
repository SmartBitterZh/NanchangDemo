$(document).ready(function() {
        function anchorClick(link) {        
        var linkSplit = link.split('/').pop();        
        $.get( linkSplit, function(data) {
            $('#content').html(data);
        });    
    }
    
    $('#container').on('click', 'a', function(e) {
        var url = $(this).attr('href');
        var name = url.split('.')[0];  
        // window.history.pushState({title:'test',url: url}); 
        window.history.pushState(name,name,url);
        anchorClick(url);        
        e.preventDefault();
    });    

    window.addEventListener('popstate', function(e) {        
        anchorClick(location.pathname);
    });    
    
    var play = false;    
    $('.play').click(function() {        
        if(play == false) {            
            $('#music')[0].play();
            play = true;
            $('.play .plbtn').hide();
            $('.play .pabtn').show();            
        } else {            
            $('#music')[0].pause();
            play = false;
            $('.play .plbtn').show();
            $('.play .pabtn').hide();        
        }
    });
});